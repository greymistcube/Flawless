using Flawless.Battle.Skill;
using UnityEngine;
using UnityEngine.UI;

namespace Flawless.UI
{
    public class StatView : MonoBehaviour
    {
        [SerializeField]
        private Slider hpBar;

        [SerializeField]
        private Text hpText;

        [SerializeField]
        private Text attackText;

        [SerializeField]
        private Text defenseText;

        [SerializeField]
        private Text speedText;

        [SerializeField]
        private Text lifestealText;

        public void UpdateView(SkillLog.CharacterStatus stat)
        {
            var hp = Mathf.Clamp(stat.hp, 0, stat.baseHp);
            hpBar.value = (float) hp / stat.baseHp;
            hpText.text = $"{hp}/{stat.baseHp}";
            attackText.text = $"���ݷ� : {stat.atk}";
            defenseText.text = $"���� : {stat.def}";
            speedText.text = $"���ǵ� : {stat.spd}";
            lifestealText.text = $"���� : {stat.lifesteal}%";
        }
    }
}

