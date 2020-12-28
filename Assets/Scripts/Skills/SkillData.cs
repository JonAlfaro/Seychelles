[System.Serializable]
public class SkillData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconName { get; set; }
    public EffectType EffectType { get; set; }
    public TargetType TargetType { get; set; }

    public SkillData(int id, string name, string description, string iconName, EffectType effectType, TargetType targetType)
    {
        Id = id;
        Name = name;
        Description = description;
        IconName = iconName;
        EffectType = effectType;
        TargetType = targetType;
    }
}

public enum EffectType
{
    damage,
    heal,
    revive,
}

public enum TargetType
{
    self,
    allyRandom,
    allyAOE,
    enemy,
    enemyAOE,
}