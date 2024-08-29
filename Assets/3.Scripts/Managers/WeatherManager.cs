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

    [SerializeField]
    private List<WeatherData> weatherData;
    private Dictionary<int, WeatherData> ableWeather = new Dictionary<int, WeatherData>();

    private int[] weatherIndex = new int[6];
    private int weatherHour;

    private int hour = 0;
    private int Hour
    {
        get
        {
            return hour;
        }
        set
        {
            if(hour != value)
            {
                WeatherTime(ref weatherHour, hour);
            }
            hour = value;
        }
    }
    private int second;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        AbleWeatherList();
        GenerateWeather(ableWeather);

        for(int i = 0; i < weatherIndex.Length; i++)
        {
            weatherIndex[i] = 0;
        }
    }

    private void WeatherTime(ref int weatherHour, int hour)
    {
        weatherHour -= hour;
    }

    public override void AbleWeatherList()
    {
        foreach (var weather in weatherData)
        {
            if (UIManager.Instance.Day <= weather.GenerateDate && !weather.IsAdded)
            {
                ableWeather.Add(weather.WeatherNum, weather);
                weather.IsAdded = true;
            }
        }

        GenerateWeather(ableWeather);
    }

    public void GenerateWeather(Dictionary<int, WeatherData> weather)
    {
        foreach(var final in weather)
        {
            if(final.Value.GenerateTerm == weatherIndex[final.Key - 1])
            {
                weatherIndex[final.Key - 1] = 0;
                if(Random.Range(0.00f, 1.00f) >= final.Value.WeatherPercent)
                {
                    weatherHour = Random.Range(3, final.Value.GenerateTime);
                    Instantiate(final.Value.WeatherObject, Camera.main.transform);
                    while(weatherHour >= 0)
                    {
                        float gameTime = Time.deltaTime * 60;
                        second = Mathf.FloorToInt(gameTime);
                        Hour = (second / 3600) % 24;
                    }
                }
            }
            else
            {
                weatherIndex[final.Key - 1]++;
            }
        }
    }
}
