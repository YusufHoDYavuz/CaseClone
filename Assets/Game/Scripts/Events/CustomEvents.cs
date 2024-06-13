using UnityEngine;

public struct UpdateWeapon
{
    public int weaponId;

    public UpdateWeapon(int weaponId)
    {
        this.weaponId = weaponId;
    }
}

public struct GetCharacter
{
    public GameObject character;

    public GetCharacter(GameObject character)
    {
        this.character = character;
    }
}