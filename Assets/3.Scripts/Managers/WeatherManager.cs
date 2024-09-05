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
            if(hour != value && ableWeather.Count > 0)
            {
                WeatherTime(ref weatherHour);
                hour = value;
            }
            else
            {
                hour = 0;
            }
        }
    }
    private int second;

    private bool isGenerated = false;

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

    private void Update()
    {
        float gameTime = Time.deltaTime * 60;
        second = Mathf.FloorToInt(gameTime);
        Hour = (second / 3600) % 24;
    }

    private void WeatherTime(ref int weatherHour)
    {
        weatherHour -= 1;
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
        foreach(var finalWeather in weather)
        {
            if(finalWeather.Value.GenerateTerm == weatherIndex[finalWeather.Key - 1])
            {
                weatherIndex[finalWeather.Key - 1] = 0;
                if(Random.Range(0.00f, 1.00f) >= finalWeather.Value.WeatherPercent && !isGenerated)
                {
                    weatherHour = Random.Range(3, finalWeather.Value.GenerateTime);
                    GameObject gameObject = finalWeather.Value.WeatherObject;
                    Instantiate(gameObject, Camera.main.transform);
                    isGenerated = true;
                    if(weatherHour <= 0)
                    {
                        Destroy(gameObject);
                        isGenerated = false;
                    }
                }
            }
            else
            {
                weatherIndex[finalWeather.Key - 1]++;
            }
        }
    }
}