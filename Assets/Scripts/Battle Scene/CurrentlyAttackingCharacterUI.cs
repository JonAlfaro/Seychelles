using UnityEngine;
using UnityEngine.Assertions;

public class CurrentlyAttackingCharacterUI : MonoBehaviour
{
    public GameObject Target;
    public GameObject[] TargetPositions;
    public AutoAttackManager AutoAttackManager;

    private void Awake()
    {
        Assert.IsNotNull(Target);
        Assert.IsNotNull(AutoAttackManager);
        Assert.IsTrue(TargetPositions.Length > 0);
    }

    private void Start()
    {
        UpdateTargetPosition();
    }

    public void UpdateTargetPosition()
    {
        int positionIndex = AutoAttackManager.AttackingCharacterIndex;
        Transform targetPositionTransform = TargetPositions[positionIndex].transform;
        Target.transform.SetPositionAndRotation(targetPositionTransform.position, targetPositionTransform.rotation);
    }
}
