﻿// This file has been modified by Microsoft on 9/2017.

using GoogleTestAdapter.Settings;
using System;

namespace GoogleTestAdapter.VsPackage.OptionsPages
{

    public class GoogleTestOptionsDialogPage : NotifyingDialogPage
    {
        #region Runtime behavior

        [LocalizedCategory("CategoryRuntimeBehaviorName")]
        [LocalizedDisplayName("OptionCatchExceptions")]
        [LocalizedDescription("OptionCatchExceptionsDescription")]
        public bool CatchExceptions
        {
            get { return _catchExceptions; }
            set { SetAndNotify(ref _catchExceptions, value); }
        }
        private bool _catchExceptions = SettingsWrapper.OptionCatchExceptionsDefaultValue;

        [LocalizedCategory("CategoryRuntimeBehaviorName")]
        [LocalizedDisplayName("OptionBreakOnFailure")]
        [LocalizedDescription("OptionBreakOnFailureDescription")]
        public bool BreakOnFailure
        {
            get { return _breakOnFailure; }
            set { SetAndNotify(ref _breakOnFailure, value); }
        }
        private bool _breakOnFailure = SettingsWrapper.OptionBreakOnFailureDefaultValue;

        #endregion

        #region Test execution

        [LocalizedCategory("CategoryTestExecutionName")]
        [LocalizedDisplayName("OptionRunDisabledTests")]
        [LocalizedDescription("OptionRunDisabledTestsDescription")]
        public bool RunDisabledTests
        {
            get { return _runDisabledTests; }
            set { SetAndNotify(ref _runDisabledTests, value); }
        }
        private bool _runDisabledTests = SettingsWrapper.OptionRunDisabledTestsDefaultValue;

        [LocalizedCategory("CategoryTestExecutionName")]
        [LocalizedDisplayName("OptionNrOfTestRepetitions")]
        [LocalizedDescription("OptionNrOfTestRepetitionsDescription")]
        public int NrOfTestRepetitions
        {
            get { return _nrOfTestRepetitions; }
            set
            {
                if (value < -1)
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Expected a number greater than or equal to -1.");
                SetAndNotify(ref _nrOfTestRepetitions, value);
            }
        }
        private int _nrOfTestRepetitions = SettingsWrapper.OptionNrOfTestRepetitionsDefaultValue;

        [LocalizedCategory("CategoryTestExecutionName")]
        [LocalizedDisplayName("OptionShuffleTests")]
        [LocalizedDescription("OptionShuffleTestsDescription")]
        public bool ShuffleTests
        {
            get { return _shuffleTests; }
            set { SetAndNotify(ref _shuffleTests, value); }
        }
        private bool _shuffleTests = SettingsWrapper.OptionShuffleTestsDefaultValue;

        [LocalizedCategory("CategoryTestExecutionName")]
        [LocalizedDisplayName("OptionShuffleTestsSeed")]
        [LocalizedDescription("OptionShuffleTestsSeedDescription")]
        public int ShuffleTestsSeed
        {
            get { return _shuffleTestsSeed; }
            set
            {
                GoogleTestConstants.ValidateShuffleTestsSeedValue(value);
                SetAndNotify(ref _shuffleTestsSeed, value);
            }
        }
        private int _shuffleTestsSeed = SettingsWrapper.OptionShuffleTestsSeedDefaultValue;

        #endregion
    }

}