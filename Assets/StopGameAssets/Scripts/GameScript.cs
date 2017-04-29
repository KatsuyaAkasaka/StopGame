using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

	public Text timeLabel;		//出力する時間
	public Text judge;			//出力するテキスト
	float timer = 0.0f;				//時間
	public float goalTime;
	private bool StartFlag = false;		//スタート押したかどうか

	private float excellentTime = 0.05f;	//判定
	private float greatTime = 0.1f;		//
	private float niceTime = 0.15f;			//
	public float judgeScore;	//周期
	public int score = 0;			//スコア

	private float difference;		//タイムの差
	private float manyPush = 0.0f;	//前回押した時間

	private int r, g, b;		//RGB値
	private int changeNumber;	//一回で減るRGB値(固定)
	private int changeArea = 0;	//色彩変化させる段階
	private int increaceNumber;	//一回で増えるRGB値(変動)
	public Material ColorMat;	//色を変化させるMaterial


	// Use this for initialization
	void Start () {
		r = 0;	g = 0;	b = 255;
		timeLabel.text = "StopGame";
		changeNumber = 255 * 4 / (int)(goalTime / judgeScore);
		ColorMat.color = new Color (0f,0f,1f);

	}
	
	// Update is called once per frame
	void Update () {
		if (timer < goalTime) {
			if (StartFlag) {
				timer += Time.deltaTime;
				timeLabel.text = timer.ToString ("f1");
			}
		}
	}

	public void StartButton(){
		StartFlag = true;
	}

	public void StopButton(){
		//連打防止
		if (Time.time - manyPush > 1.0f) {
			//差を判定
			difference = (timer % judgeScore) < (judgeScore - timer % judgeScore) 
				? timer % judgeScore 
				: judgeScore - timer % judgeScore;
			
			manyPush = Time.time;

			Debug.Log (difference);

			if (difference < excellentTime) {
				judge.text = "Excellent!!";
				score = 3;
			} else if (difference < greatTime) {
				judge.text = "Great!";
				score = 2;
			} else if (difference < niceTime) {
				judge.text = "Nice";
				score = 1;
			} else {
				judge.text = "Miss...";
				score = 0;
			}
			if(score > 0)
				BallColorChanger ();
		}
	}

	public void BallColorChanger(){
		increaceNumber = changeNumber * score;
		if (changeArea == 0) {
			g += increaceNumber;
			if (g >= 255) {
				changeArea++;
				increaceNumber = g - 255;
				g = 255;
			}
		} 
		if (changeArea == 1) {
			b -= increaceNumber;
			if (b <= 0) {
				changeArea++;
				increaceNumber = -b;
				b = 0;
			}
		}
		if (changeArea == 2) {
			r += increaceNumber;
			if (r >= 255) {
				changeArea++;
				increaceNumber = r - 255;
				r = 255;
			}
		} 
		if (changeArea == 3) {
			g -= increaceNumber;
			if (g <= 0) {
				changeArea++;
				increaceNumber = -g;
				g = 0;
			}
		}

		ColorMat.color = new Color ((float)r / 255f, (float)g / 255f, (float)b / 255f);

		if (r == 255 && g == 0 && b == 0) {
			judge.text = "Clear!!";
			StartFlag = false;
		}
	}
}
