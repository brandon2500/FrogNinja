using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Numerics;
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

    internal async Task<int> GetRank()
    {
        string Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();

        var contract = ThirdwebManager.Instance.SDK.GetContract("0x8F3FfaB5531F653B5c1866Cf439a6078f56cE4aa", "[{\"type\":\"event\",\"name\":\"ScoreAdded\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"score\",\"indexed\":false,\"internalType\":\"uint256\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"function\",\"name\":\"_scores\",\"inputs\":[{\"type\":\"address\",\"name\":\"\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"getRank\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"rank\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"submitScore\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"score\",\"internalType\":\"uint256\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"}]");

        var rank = await contract.Read<int>("getRank", Address);

        return rank;
    }

    internal async Task SubmitScore(float score)
    {
        var contract = ThirdwebManager.Instance.SDK.GetContract("0x8F3FfaB5531F653B5c1866Cf439a6078f56cE4aa", "[{\"type\":\"event\",\"name\":\"ScoreAdded\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"score\",\"indexed\":false,\"internalType\":\"uint256\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"function\",\"name\":\"_scores\",\"inputs\":[{\"type\":\"address\",\"name\":\"\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"getRank\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"rank\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"submitScore\",\"inputs\":[{\"type\":\"uint256\",\"name\":\"score\",\"internalType\":\"uint256\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"}]");

        await contract.Write("submitScore", (int)score);    
    }
}