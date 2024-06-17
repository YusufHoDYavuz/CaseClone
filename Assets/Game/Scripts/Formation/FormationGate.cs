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
            SetRandomMultiplierValue();
        }

        void Update()
        {

        }

        private void SetRandomMultiplierValue()
        {
            int randomValue = Random.Range(0, multiplierAmounts.Count);
            multiplierValue = multiplierAmounts[randomValue];

            if (isIncrease)
                multiplierText.text = "+" + multiplierValue.ToString();
            else
                multiplierText.text = "-" + multiplierValue.ToString();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Character>() !=null)
            {
                EventBus<UpdateCharacterFormation>.Emit(this, new UpdateCharacterFormation(isIncrease, multiplierValue));
                Destroy(gameObject);
            }
        }
    }
}
