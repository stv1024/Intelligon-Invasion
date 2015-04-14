//using UnityEngine;

///// <summary>
///// 金币显示框，可以有动画哦
///// </summary>
//public class CoinLabel : RealTime
//{
//    /// <summary>
//    /// 数字翻动动画最大长度，防止动画时间太长
//    /// </summary>
//    private const float AnimationDurationMax = 1.6f;
//    /// <summary>
//    /// 标准情况每秒变5
//    /// </summary>
//    private const float AnimationSpeedMin = 10;

//    private float _currentDuration;
//    private float _animStartTime;
//    private int _animStartValue, _animEndValue;
//    private int _currentValue;

//    public UILabel Label;

//    private void OnEnable()
//    {
//        ForeverDataHolder.DidCoinChange += OnCoinChange;
//        _animEndValue = ForeverDataHolder.Coin;
//        _currentValue = _animEndValue;
//        _animStartTime = -1;
//        RefreshLabel();
//    }

//    void OnDisable()
//    {
//        ForeverDataHolder.DidCoinChange -= OnCoinChange;
//    }


//    public void OnCoinChange(int from, int to)
//    {
//        _animEndValue = to;

//        _currentValue += Mathf.RoundToInt(Mathf.Sign(_animEndValue - _currentValue));//瞬间先变一个数字
//        RefreshLabel();
        
//        _animStartValue = _currentValue;
//        _currentDuration = Mathf.Min(Mathf.Abs(_animEndValue - _animStartValue)/AnimationSpeedMin, AnimationDurationMax);
//        _animStartTime = time;
//    }

//    private void Update()
//    {
//        if (time - _animStartTime > _currentDuration)
//        {
//            if (_currentValue != _animEndValue)
//            {
//                _currentValue = _animEndValue;
//                RefreshLabel();
//            }
//            return;
//        }
//        _currentValue =
//            Mathf.RoundToInt(Mathf.Lerp(_animStartValue, _animEndValue, (time - _animStartTime)/_currentDuration));
//        RefreshLabel();
//    }

//    void RefreshLabel()
//    {
//        Label.text = _currentValue.ToString();
//    }

//    /// <summary>
//    /// 点击加号
//    /// </summary>
//    public void OnPlusClick()
//    {
//        RechargePanel.Load();
//    }
//}