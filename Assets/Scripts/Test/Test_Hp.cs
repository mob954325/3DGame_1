using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Hp : TestInput
{
    public Player player;
    public EnemyBase enemy;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            player.HP--;
        }
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            enemy.HP--;
        }
    }
    protected override void OnTest3(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            enemy.Toughness -= 20;
        }
    }
}
