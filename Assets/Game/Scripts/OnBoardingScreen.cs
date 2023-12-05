using UnityEngine;

public class OnBoardingScreen : MonoBehaviour {
    private BaseOnBoardingList[] _baseOnBoardingList;
    public int ListLength => _baseOnBoardingList.Length;

    public void Init() {
        _baseOnBoardingList = transform.GetComponentsInChildren<BaseOnBoardingList>(true);

        for (int i = 0; i < _baseOnBoardingList.Length; i++) {
            _baseOnBoardingList[i].Init();
            _baseOnBoardingList[i].SetActive(false);
        }
    }

    public void SetActive(int index, bool state) {
        _baseOnBoardingList[index].SetActive(state);
    }
}
