using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using UnityEngine;
using System;

public class BackendService {
    private HttpClient _client;
    public int ZeroOneValue { get; private set; } = 1;

    public BackendService() {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://kokwerty.space");
    }

    public async UniTask LoadValueFromServer() {
        try {
            var loginData1 = new Dictionary<string, string>
            {
                {"token","90b966c1-fc45-4212-b9f1-4caf60517365" }
            };
            ZeroOneValue = int.Parse(await GetData(loginData1, "/app/av1ag4me"));
        }
        catch (Exception e) {
            Debug.Log(e);
        }
    }

    private async UniTask<string> GetData(Dictionary<string, string> data, string postName) {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, postName) {
            Content = content
        };

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}