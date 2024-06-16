using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thirdweb;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;

public class BlockchainManagerScript : MonoBehaviour
{
    public UnityEvent<string> OnLoggedIn;

    public string Address { get; private set; }

    public static BlockchainManagerScript Instance { get; private set; }


    void Start()
    {
        var sdk = ThirdwebManager.Instance.SDK;
    }

    void Update()
    {

    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public async void Login(string authProvider)
    {
        AuthProvider provider = AuthProvider.Google;
        switch (authProvider)
        {
            case "google":
                provider = AuthProvider.Google;
                break;
            case "apple":
                provider = AuthProvider.Apple;
                break;
            case "facebook":
                provider = AuthProvider.Facebook;
                break;
        }

        var connection = new WalletConnection(
            provider: WalletProvider.SmartWallet,
            chainId: 59141,
            authOptions: new AuthOptions(authProvider: provider)
        );

        Address = await ThirdwebManager.Instance.SDK.Wallet.Connect(connection);

        InvokeOnLoggedIn();

    }

    void InvokeOnLoggedIn()
    {
        OnLoggedIn.Invoke(Address);
    }

}