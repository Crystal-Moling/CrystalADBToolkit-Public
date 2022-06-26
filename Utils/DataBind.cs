using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace CrystalADBToolkit.Utils
{
    public class DataBind
    {
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

        #region Hitokoto

            private static Visibility _isHitokotoEnabled;
            private static String _hitokotoText;
            private static String _hitokotoUuidInfo;
            private static String _hitokotoFromInfo;
            public static Visibility IsHitokotoEnabled
            {
                get => _isHitokotoEnabled;
                set
                {
                    _isHitokotoEnabled = value;
                    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(IsHitokotoEnabled)));
                }
            }
            public static String HitokotoText
            {
                get => _hitokotoText;
                set
                {
                    _hitokotoText = value;
                    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(HitokotoText)));
                    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(HitokotoTextInfo)));
                }
            }
            public static String HitokotoTextInfo => "『" + _hitokotoText + "』";
            public static String HitokotoUuidInfo
            {
                get => _hitokotoUuidInfo;
                set
                {
                    _hitokotoUuidInfo = value;
                    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(HitokotoUuidInfo)));
                }
            }
            public static String HitokotoFromInfo
            {
                get => _hitokotoFromInfo;
                set
                {
                    _hitokotoFromInfo = "——「" + value + "」";
                    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(HitokotoFromInfo)));
                }
            }

        #endregion

        #region Background

            private static ImageSource _background;
            private static ImageSource _userTile;
            public static ImageSource Background
            {
                get => _background;
                set
                {
                    _background = value;
                    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(Background)));
                }
            }
            public static ImageSource UserTile
            {
                get => _userTile;
                set
                {
                    _userTile = value;
                    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(UserTile)));
                }
            }

        #endregion

        #region DeviceInfo

            private static String _deviceBrand;
            private static String _deviceModel;
            private static String _deviceAndroidId;
            private static String _deviceAndroidVersion;
            private static String _deviceSdkVersion;
            private static String _deviceSecurityPatch;
            private static String _deviceName;
            private static String _deviceBoard;
            public static String DeviceBrand
            {
                get => _deviceBrand;
                set
                {
                    _deviceBrand = value;
                    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(DeviceBrand)));
                }
            }
            public static String DeviceModel
            {
                get => _deviceModel;
                set
                {
                    _deviceModel = value;
                    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(DeviceModel)));
                }
            }
            public static String DeviceAndroidId
            {
                get => _deviceAndroidId;
                set
                {
                    _deviceAndroidId = value;
                    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(DeviceAndroidId)));
                }
            }
            public static String DeviceAndroidVersion
            {
                get => _deviceAndroidVersion;
                set
                {
                    _deviceAndroidVersion = value;
                    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(DeviceAndroidVersion)));
                }
            }
            public static String DeviceSdkVersion
            {
                get => _deviceSdkVersion;
                set
                {
                    _deviceSdkVersion = value;
                    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(DeviceSdkVersion)));
                }
            }
            public static String DeviceSecurityPatch
            {
                get => _deviceSecurityPatch;
                set
                {
                    _deviceSecurityPatch = value;
                    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(DeviceSecurityPatch)));
                }
            }
            public static String DeviceName
            {
                get => _deviceName;
                set
                {
                    _deviceName = value;
                    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(DeviceName)));
                }
            }
            public static String DeviceBoard
            {
                get => _deviceBoard;
                set
                {
                    _deviceBoard = value;
                    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(DeviceBoard)));
                }
            }

        #endregion

        #region Apps

            private static List<String> _packagesList;

            public static List<String> PackagesList
            {
                get => _packagesList;
                set
                {
                    _packagesList = value;
                    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(PackagesList)));
                }
            }

        #endregion
    }
}