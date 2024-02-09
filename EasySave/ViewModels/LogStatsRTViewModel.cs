using System;
using System.IO;
using System.ComponentModel;
using EasySave.Services.Common;
using EasySave.ViewModels.SaveProcess;
using EasySave.Models.StatsRTModelNameSpace;
using EasySave.Services;

namespace EasySave.ViewModels.LogStatsRTViewModelNameSpace
{
    public class LogStatsRTViewModel
    {
        private StatsRTModel _currentStatsRTModel;
        private WriteStatsRT _writeJson = new WriteStatsRT();
        public WriteStatsRT WriteJson
        {
            get => _writeJson;
        }

        public void NewWork(StatsRTModel _StatsRTModel)
        {
            if (_currentStatsRTModel != null)
            {
                _currentStatsRTModel.PropertyChanged -= OnStatsRTModelPropertyChanged;
            }

            // Attachement à la nouvelle instance.
            _currentStatsRTModel = _StatsRTModel ?? throw new ArgumentNullException(nameof(_StatsRTModel));
            _currentStatsRTModel.PropertyChanged += OnStatsRTModelPropertyChanged;
        }

        private void OnStatsRTModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Réagir au changement de propriété ici
            if (e.PropertyName == nameof(StatsRTModel.State))
            {
                HandleStateChange(_currentStatsRTModel);
            }
        }
        private void HandleStateChange(StatsRTModel state)
        {
            string JsonPath = @"C:\Users\ProSoft\Downloads\save\Json";
            _writeJson.WriteRealTimeStatsAsync(state, JsonPath);
        }
    }
}