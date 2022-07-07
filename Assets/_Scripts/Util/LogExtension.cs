using Flawless.Battle.Skill;
using System.Text;

namespace Flawless.Util
{
    public static class LogExtension
    {
        public static string GetSkillDescription(string skillName)
        {
            var name = LocalizationManager.Instance.GetSkillName(skillName);
            var desc = LocalizationManager.Instance.GetSkillDescription(skillName);
            return $"{name} : {desc}";
        }

        public static string PoseToString(this PoseType type)
        {
            switch (type)
            {
                case PoseType.High:
                    return "���� �ڼ�";
                case PoseType.Low:
                    return "���� �ڼ�";
                case PoseType.Special:
                    return "��� �ڼ�";
            }

            return null;
        }

        public static string SkillLogToString(this SkillLog actionLog)
        {
            var sb = new StringBuilder();
            sb.Append($"[Turn {actionLog.TurnCount + 1}] {actionLog.CasterName} : ");

            if (actionLog.Skill == null)
            {
                sb.AppendLine("�ƹ� �ൿ�� ���� �ʾҴ�.");
                return sb.ToString();
            }

            var skillName = LocalizationManager.Instance.GetSkillName(actionLog.Skill.GetType().Name);
            sb.AppendLine($"{skillName} ��ų�� ����Ͽ���. (���ǵ� : {actionLog.Speed})");
            if (actionLog.Blocked)
            {
                sb.AppendLine("��ų ����� ���ܵǾ���.");
                return sb.ToString();
            }
            if (actionLog.BlockedByCounter)
            {
                sb.AppendLine("��ų ����� ī���ͷ� ���� ���ܵǾ���.");
                return sb.ToString();
            }
            if (actionLog.BlockedByPose)
            {
                sb.AppendLine("��ų �ߵ��� ���� �ڼ��� ���� �ʾ� �ߵ��� �� ������.");
                return sb.ToString();
            }
            if (actionLog.BlockedByCooldown)
            {
                sb.AppendLine("��Ÿ�� ���̶� �ߵ��� �� ������.");
                return sb.ToString();
            }
            if (actionLog.DamageBlocked)
            {
                sb.AppendLine("ī���ͷ� ���� �������� ������ ���ߴ�.");
            }
            if (actionLog.DealtDamage > 0)
            {
                sb.AppendLine($"{actionLog.DealtDamage}�� �������� ������. [������ ���� : {actionLog.DamageMultiplier}��]");
            }
            if (actionLog.LifestealAmount > 0)
            {
                sb.AppendLine($"{actionLog.LifestealAmount} HP�� ����ߴ�.");
            }
            if (actionLog.HealAmount > 0)
            {
                sb.AppendLine($"{actionLog.HealAmount} HP�� ȸ���ߴ�.");
            }
            if (actionLog.CounteredDamage > 0)
            {
                sb.AppendLine($"�ݰݴ��� {actionLog.HealAmount}�� �������� �Ծ���.");
            }

            return sb.ToString();
        }
    }
}
