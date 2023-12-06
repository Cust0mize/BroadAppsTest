using Game.Scripts.Signal;
using Zenject;

public class OnBoardingPanel : UIPanel {
    private OnBoardingScreen[] _onBoardingScreen;
    private int _currentScreenIndex;
    private long _onboardingIndex = 1;

    private BackendService _backendService;
    private SignalBus _signalBus;
    private GameData _gameData;

    [Inject]
    private void Construct(
        BackendService backendService,
        SignalBus signalBus,
        GameData gameData
    ) {
        _backendService = backendService;
        _gameData = gameData;
        _signalBus = signalBus;
        signalBus.Subscribe<SignalNextOnBoardingClick>(OnBoardingScreenUpdate);
    }

    private void OnBoardingScreenUpdate(SignalNextOnBoardingClick signalNextOnBoardingClick) {
        _onBoardingScreen[_onboardingIndex].SetActive(_currentScreenIndex, false);
        _currentScreenIndex++;
        if (_currentScreenIndex < _onBoardingScreen[_onboardingIndex].ListLength) {
            _onBoardingScreen[_onboardingIndex].SetActive(_currentScreenIndex, true);
        }
        else {
            if (_onboardingIndex == 0) {
                _gameData.IsShowOnboarding = false;
            }
            _signalBus.Fire(new SignalGameIsLoad());
        }
    }

    private void Start() {
        if (_gameData.IsShowOnboarding) {
            //_onboardingIndex = FirebaseRemoteConfig.DefaultInstance.GetValue("_isReview").LongValue;            
            _onboardingIndex = _backendService.ZeroOneValue;
            print(_onboardingIndex);

            _onBoardingScreen = transform.GetComponentsInChildren<OnBoardingScreen>(true);

            for (int i = 0; i < _onBoardingScreen.Length; i++) {
                _onBoardingScreen[i].Init();
                _onBoardingScreen[i].gameObject.SetActive(false);
            }
            _onBoardingScreen[_onboardingIndex].gameObject.SetActive(true);
            _onBoardingScreen[_onboardingIndex].SetActive(_currentScreenIndex, true);
        }
    }
}
