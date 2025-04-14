using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectManager : MonoBehaviour
{
    public static EffectManager effectManager { get; private set; }

    [Header("Main Variables")]
    [SerializeField] private Dictionary<int, Action> effects = new Dictionary<int, Action>();

    private void Awake()
    {
        /// Singleton mechanism
        {
            if (effectManager != null && effectManager != this)
            {
                Destroy(this);
            }
            else
            {
                effectManager = this;
            }
        }

    }

    private void Start()
    {
        effects[0] = () => Effects.NoEffect(); // no effect; default
        effects[1] = () => Debug.Log("1");
        effects[2] = () => Debug.Log("2");
        effects[3] = () => Debug.Log("3");
        effects[4] = () => Debug.Log("4");
        effects[5] = () => Debug.Log("5");
        effects[6] = () => Debug.Log("6");
        effects[7] = () => Debug.Log("7");
        effects[8] = () => Debug.Log("8");
        effects[9] = () => Debug.Log("9");
        effects[10] = () => Debug.Log("10");
        effects[11] = () => Debug.Log("11");
        effects[12] = () => Debug.Log("12");
    }

    public void TriggerCardEffect(int effectId)
    {
        effects[effectId]();
    }

    class Effects
    {
        static public void NoEffect()
        {
            Debug.Log("No effect!");
        }

        static public void TheEmperor()
        {
            // something something, can't be stolen
        }

        // more effects added here
        // static public void <cardName>() {   <effect>    }

    }

    

}
