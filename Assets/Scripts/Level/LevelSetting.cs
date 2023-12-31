using System;
using UnityEngine;
using Zenject;

public class LevelSetting : MonoBehaviour
{
    public event Action<SettingData> OnChangeLevelSetting;

    [SerializeField] private SettingData[] _levelSetting;
    [SerializeField] private int _currentIndexSetting = 0;
    [SerializeField] private int _nextUpdateIndex = 10;

    private NumberSelect _numberSelect;
    private int _counterNextUpdate = 0;

    [Inject]
    private void Construct(NumberSelect numberSelect)
    {
        _numberSelect = numberSelect;

        _numberSelect.OnSelectComplete += SelectedComplete;
    }

    private void OnDestroy() => _numberSelect.OnSelectComplete -= SelectedComplete;

    public SettingData GetCurrentSetting() => _levelSetting[_currentIndexSetting];

    public void RestartSetting()
    {
        _currentIndexSetting = 0;
        _counterNextUpdate = 0;
        OnChangeLevelSetting?.Invoke(GetCurrentSetting());
    }

    private void UpdateIndexSetting()
    {
        if (_currentIndexSetting + 1 >= _levelSetting.Length) return;

        _currentIndexSetting++;
        OnChangeLevelSetting?.Invoke(GetCurrentSetting());
    }

    private void SelectedComplete()
    {
        _counterNextUpdate++;
        if(_counterNextUpdate >= _nextUpdateIndex)
        {
            UpdateIndexSetting();

            _counterNextUpdate = 0;
        }
    }
}