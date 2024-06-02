using System.Collections;
using UnityEngine;

using AlphaReturns.System.Unit;

namespace AlphaReturns.System.Battle
{
    public class BattleSystem : MonoBehaviour 
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject enemyPrefab;

        [SerializeField] private Transform playerBattleStation;
        [SerializeField] private Transform enemyBattleStation;

        [SerializeField] private Characters playerCharacter;
        [SerializeField] private Characters enemyCharacter;

        [SerializeField] private BattleHUD playerHUD;
        [SerializeField] private BattleHUD enemyHUD;

        [SerializeField] private BattleState state;

        // Start is called before the first frame update
        void Start()
        {
            state = BattleState.START;
            StartCoroutine(SetupBattle());
        }

        IEnumerator SetupBattle()
        {
            GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
            playerCharacter = playerGO.GetComponent<Characters>();

            GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
            enemyCharacter = enemyGO.GetComponent<Characters>();

            playerHUD.SetHUD(playerCharacter);
            enemyHUD.SetHUD(enemyCharacter);

            yield return new WaitForSeconds(2f);

            state = BattleState.PLAYERTURN;
        }
    }
}