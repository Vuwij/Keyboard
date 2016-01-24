using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	AudioSource audiosource;

	public AudioClip[] audioclips;

	public ThalmicMyo thalmicMyo;

	public GameObject middlefinger;

	bool latch;
	void Start () {
		audiosource = GetComponent<AudioSource> ();
		latch = false;
	}

	Thalmic.Myo.Pose currentpose, nextpose;
	void Update () {
		if (currentpose == Thalmic.Myo.Pose.Rest && (nextpose == Thalmic.Myo.Pose.Fist || nextpose == Thalmic.Myo.Pose.WaveIn) && !latch) {
			latch = true;
			StartCoroutine (playsound());
		}
		currentpose = nextpose;
		nextpose = thalmicMyo.pose;
	}

	bool ifMultipleNotes = true;

	private IEnumerator playsound() {

		if (!ifMultipleNotes)
			audiosource.Play ();
		else
			playMoreNotes ();
		yield return new WaitForSeconds (0.2f);
		latch = false;
		yield break;
	}

	RaycastHit hit;
	float distanceToGround = 0;

	private void playMoreNotes() {
		Debug.DrawRay (middlefinger.transform.position, Vector3.down*10, Color.red, 10);
		if (Physics.Raycast(middlefinger.transform.position, Vector3.down*10, out hit)) {
			var keyName = hit.transform.gameObject.name;
			Debug.Log (keyName);
			foreach (AudioClip a in audioclips) {
				if (a.name == keyName) {
					audiosource.clip = a;
					audiosource.Play ();
				}
			}
		}
	}
}
