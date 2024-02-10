using System;
using System.IO;
using System.ComponentModel;
using EasySave.ViewModels;
using EasySave.Models;
using EasySave.Services;

namespace EasySave.ViewModels
{
    public class LogStatsRTViewModel
    {
        private StatsRTModel _currentStatsRTModel;
        //private WriteStatsRT _writeJson = new WriteStatsRT();
        //public WriteStatsRT WriteJson
        //{
        //    get => _writeJson;
        //}

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
//            WriteStatsRT.QueueStatsForWriting(state);
            WriteStatsRT.WriteRealTimeStatsAsync(state, JsonPath);
        }
        public void WriteLog(LogModel log)
        {
            string JsonPath = @"C:\Users\ProSoft\Downloads\save\Json";
            WriteStatsRT.WriteLogsAsync(log, JsonPath);
        }
    }
}