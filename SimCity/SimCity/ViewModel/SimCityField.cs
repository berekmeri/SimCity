using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimCity_Model;

namespace SimCity.ViewModel
{
    public class SimCityField : ViewModelBase
    {
        #region Fields
        private string _text = string.Empty;
        private SimCity_Model.Model.FieldType _fieldType;
        private int _zoneIndex;
        private SimCity_Model.Model.ZoneType _zoneType;
        private bool _isCatastrophe;
        #endregion

        #region Properties
        public int X { get; set; }
        public int Y { get; set; }
        public int Index { get; set; }
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }
        public SimCity_Model.Model.FieldType FieldType
        {
            get { return _fieldType; }
            set
            {
                if (_fieldType != value)
                {
                    _fieldType = value;
                    OnPropertyChanged();
                }
            }
        }
        public SimCity_Model.Model.ZoneType ZoneType
        {
            get { return _zoneType; }
            set
            {
                if (_zoneType != value)
                {
                    _zoneType = value;
                    OnPropertyChanged();
                }
            }
        }
        public int ZoneIndex
        {
            get { return _zoneIndex; }
            set
            {
                if (_zoneIndex != value)
                {
                    _zoneIndex = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsCatastrophe 
        { 
            get { return _isCatastrophe; }
            set 
            { 
                if(_isCatastrophe != value)
                {
                    _isCatastrophe = value;
                    OnPropertyChanged();
                }
                
            }
        }
        #endregion

        #region Commands
        public DelegateCommand? FieldClickedCommand { get; set; }
        #endregion
    }
}
