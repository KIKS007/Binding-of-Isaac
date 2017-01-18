using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TeardropBig : Teardrop 
{
	[Header ("Settings")]
	public float newScale;
	public float growDuration;
	public float shrinkDuration;
	public Ease ease;

	private Vector3 initialScale;

	// Use this for initialization
	void Start () 
	{
		initialScale = transform.localScale;

		Grow ();
	}

	void Grow ()
	{
		if(transform != null && !dead)
			transform.DOScale (newScale, growDuration).SetEase (ease).OnComplete (Shrink);
		
		newScale += 1;
	}

	void Shrink ()
	{
		if(transform != null && !dead)
			transform.DOScale (initialScale, shrinkDuration).SetEase (ease).OnComplete (Grow);
	}

	public override void Kill ()
	{
		DOTween.Kill (transform);
		base.Kill ();
	}
}
