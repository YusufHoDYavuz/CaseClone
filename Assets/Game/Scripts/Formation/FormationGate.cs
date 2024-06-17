using System.Collections.Generic;
using TMPro;
using UnityEngine;
using XGames.GameName.EventSystem;

namespace XGames.GameName
{
    public class FormationGate : MonoBehaviour
    {
        [Header("Multiplier")]
        [SerializeField] private bool isIncrease;
        [SerializeField] private TextMeshPro multiplierText;
        [SerializeField] private List<int> multiplierAmounts;
        private int multiplierValue;

        void Start()
        {
            int randomValue = Random.Range(0, multiplierAmounts.Count);
            SetRandomMultiplierValue(randomValue);
        }

        void Update()
        {

        }

        private void SetRandomMultiplierValue(int randomValue)
        {
            multiplierValue = multiplierAmounts[randomValue];
            UpdateMultiplierText();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Character>() != null)
            {
                EventBus<UpdateCharacterFormation>.Emit(this, new UpdateCharacterFormation(isIncrease, multiplierValue));
                Destroy(gameObject);
            }
            else if (other.GetComponent<Bullet>() != null)
            {
                multiplierValue++;
                UpdateMultiplierText();
            }
        }

        private void UpdateMultiplierText()
        {
            if (isIncrease)
                multiplierText.text = "+" + multiplierValue.ToString();
            else
                multiplierText.text = "-" + multiplierValue.ToString();
        }
    }
}
