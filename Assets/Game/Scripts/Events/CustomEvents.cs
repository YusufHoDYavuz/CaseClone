using UnityEngine;

public struct UpdateWeaponEvent
{
    public int weaponId;

    public UpdateWeaponEvent(int weaponId)
    {
        this.weaponId = weaponId;
    }
}

public struct GetCharacterEvent
{
    public GameObject character;

    public GetCharacterEvent(GameObject character)
    {
        this.character = character;
    }
}

public struct UpdateCharacterFormation
{
    public bool isIncrease;
    public int updateFormationCount;

    public UpdateCharacterFormation(bool isIncrease, int updateAmount)
    {
        this.isIncrease = isIncrease;
        this.updateFormationCount = updateAmount;
    }
}
