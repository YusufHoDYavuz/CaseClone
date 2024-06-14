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
