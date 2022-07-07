using System;
using Flawless.Models.Encounters;
using Flawless.States;
using Libplanet;
using Libplanet.Action;
using Libplanet.Unity;

namespace Flawless.Actions
{
    // Used for reflection when deserializing a stored action.
    [ActionType("pub_heal_action")]
    public class PubHealAction : ActionBase
    {
        // Used for reflection when deserializing a stored action.
        public PubHealAction()
        {
        }

        // Used for serialzing an action.
        public override Bencodex.Types.IValue PlainValue => Bencodex.Types.Null.Value;

        // Used for deserializing a stored action.
        public override void LoadPlainValue(Bencodex.Types.IValue plainValue)
        {
            if (!(plainValue is Bencodex.Types.Null))
            {
                throw new ArgumentException(
                    $"Invalid {nameof(plainValue)} type: {plainValue.GetType()}");
            }
        }

        // Executes an action.
        // This is what gets called when a block containing an action is mined
        // or appended to a blockchain.
        public override IAccountStateDelta Execute(IActionContext context)
        {
            // Retrieves the previously stored state.
            IAccountStateDelta states = context.PreviousStates;
            EnvironmentState environmentState =
                states.GetState(EnvironmentState.EnvironmentAddress) is Bencodex.Types.Dictionary environmentStateEncoded
                    ? new EnvironmentState(environmentStateEncoded)
                    : throw new ArgumentException("No environment found; please run InitalizeStatesAction first.");
            PlayerState playerState =
                states.GetState(context.Signer) is Bencodex.Types.Dictionary playerStateEncoded
                    ? new PlayerState(playerStateEncoded)
                    : throw new ArgumentException($"Invalid player state at {context.Signer}.");

            Encounter encounter = playerState.SceneState.GetEncounter(environmentState);
            if (!playerState.SceneState.InEncounter)
            {
                throw new ArgumentException($"Invalid scene state at {context.Signer}: {playerState.SceneState.Encode()}");
            }
            else if (!(encounter is PubEncounter pubEncounter))
            {
                throw new ArgumentException($"Invalid encounter type at {context.Signer}: {encounter}");
            }
            else
            {
                Address weaponAddress = playerState.EquippedWeaponAddress;
                WeaponState weaponState =
                    weaponAddress == default
                        ? new WeaponState()
                        : states.GetState(playerState.EquippedWeaponAddress) is Bencodex.Types.Dictionary weaponStateEncoded
                            ? new WeaponState(weaponStateEncoded)
                            : throw new ArgumentException($"Invalid weapon state at {weaponAddress}");
                long maxHealth = playerState.GetMaxHealth(weaponState);
                long health = maxHealth - playerState.StatsState.Damages;
                long healAmount = (maxHealth * pubEncounter.HealPercentage) / 100;

                if (playerState.Gold < pubEncounter.HealPrice)
                {
                    throw new ArgumentException(
                        $"Character does not have enough gold to reset points.");
                }
                else
                {
                    playerState = playerState.Heal(healAmount).SubtractGold(pubEncounter.HealPrice);
                }
            }

            return states.SetState(context.Signer, playerState.Encode());
        }
    }
}
