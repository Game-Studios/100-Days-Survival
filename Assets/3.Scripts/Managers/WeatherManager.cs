using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : WeatherBase
{
    //��ǳ�� - 0�Ϻ��� 1�ϸ��� 15% Ȯ��
    //����(����) - 5�Ϻ��� 1�ϸ��� 10% Ȯ��
    //��ǳ - 10�Ϻ��� 3�ϸ��� 5% Ȯ��
    //������ - 20�Ϻ��� 3�ϸ��� 5% Ȯ��
    //ȭ������ - 50�� Ȯ�� ���Ŀ� 10�ϸ��� 3% 100�� Ȯ��
    //���� - 50�Ϻ��� 10�ϸ��� 3% Ȯ��


    public static WeatherManager Instance;

    private List<WeatherData> weatherData;
    private Dictionary<int, WeatherData> ableWeather;

    private int[] weatherIndex = new int[6];

    private void Start()
    {
        AbleWeatherList();
        GenerateWeather(ableWeather);
        for(int i = 0; i < weatherIndex.Length; i++)
        {
            weatherIndex[i] = 0;
        }
    }

    public override void AbleWeatherList()
    {
        foreach (var weather in weatherData)
        {
            if (UIManager.Instance.Day <= weather.GenerateDate)
            {
                ableWeather.Add(weather.WeatherNum, weather);
            }
        }
    }

    public void GenerateWeather(Dictionary<int, WeatherData> weather)
    {
        foreach(var final in weather)
        {
            if(final.Value.GenerateTerm == weatherIndex[final.Key - 1])
            {
                weatherIndex[final.Key - 1] = 0;
                //Ȯ���� ���� ���� ����
                Instantiate(final.Value.WeatherObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
            }
            else
            {
                weatherIndex[final.Key - 1]++;
            }
        }
    }
}
