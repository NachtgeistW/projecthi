using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdBanner : MonoBehaviour
{
    [SerializeField]
    private Transform[] _bannerImage;

    [SerializeField]
    private float _imageHeight = 340f;

    [SerializeField]
    private float _waitTime = 5f;

    [SerializeField]
    private float _slideTime = 1f;

    float _overSlideTime;

    float _startSlideTime;

    bool _inSlide = false;

    // Start is called before the first frame update
    void Start()
    {
        _overSlideTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (_inSlide)
        {
            if( Time.time > _overSlideTime)
            {
                _SlideBanner(_overSlideTime - (Time.time - Time.deltaTime));
                _inSlide = false;
            }
            else
            {
                _SlideBanner(Time.deltaTime);
            }
        }


        if (Time.time - _overSlideTime > _waitTime)
        {
            _inSlide = true;

            _startSlideTime = _overSlideTime + _waitTime;
            
            _SlideBanner(Time.time - _startSlideTime);

            _overSlideTime = _startSlideTime + _slideTime;
            return;
        }
    }

    void _SlideBanner(float time)
    {
        foreach (var image in _bannerImage)
        {
            image.localPosition = new Vector3(image.localPosition.x, image.localPosition.y + 340f * time / _slideTime, image.localPosition.z);

            if (image.localPosition.y > 340f)
                image.localPosition = new Vector3(image.localPosition.x, image.localPosition.y - _bannerImage.Length * 340f, image.localPosition.z);
        }
    }
}
