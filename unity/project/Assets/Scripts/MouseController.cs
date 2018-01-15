/*
 * Copyright (c) 2016 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MouseController : MonoBehaviour 
{
    public float jetpackForce = 75.0f;
    public float forwardMovementSpeed = 3.0f;
    public Transform groundCheckTransform;
    public LayerMask groundCheckLayerMask;
    public ParticleSystem jetpack;
    public Texture2D coinIconTexture;
    public AudioClip coinCollectSound;
    public AudioSource jetpackAudio;
    public AudioSource footstepsAudio;
    public ParallaxScroll parallax;

    private Animator animator;
    private bool grounded;
    private bool dead = false;
    private uint coins = 0;

    void Start () 
    {
        animator = GetComponent<Animator>();	
    }

    void FixedUpdate () 
    {
        bool jetpackActive = Input.GetButton("Fire1");
	    jetpackActive = jetpackActive && !dead;
	    if (jetpackActive) 
	    { 
	        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jetpackForce));
	    }
	    if (!dead) 
	    {
	        Vector2 newVelocity = GetComponent<Rigidbody2D>().velocity;
	        newVelocity.x = forwardMovementSpeed;
	        GetComponent<Rigidbody2D>().velocity = newVelocity;
	    }
  	    UpdateGroundedStatus();
	    AdjustJetpack(jetpackActive);
	    AdjustFootstepsAndJetpackSound(jetpackActive);
	    parallax.offset = transform.position.x;
    } 

    void UpdateGroundedStatus() 
    {
        grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        animator.SetBool("grounded", grounded);
    }

    void AdjustJetpack (bool jetpackActive) 
    {
  	    ParticleSystem.EmissionModule jpEmission = jetpack.emission;
	    jpEmission.enabled = !grounded;
	    jpEmission.rateOverTime = jetpackActive ? 300.0f : 75.0f; 
    }

    void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.CompareTag("Coins")) 
        {
	        CollectCoin(collider);
        } 
        else 
        {
            HitByLaser(collider);
	    } 
    }

    void HitByLaser(Collider2D laserCollider) 
    {
        if (!dead) 
        {
            laserCollider.gameObject.GetComponent<AudioSource>().Play();
	    }
	    dead = true;
	    animator.SetBool("dead", true);
    }

    void CollectCoin(Collider2D coinCollider) 
    {
        coins++;
        Destroy(coinCollider.gameObject);
        AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
    }

    void OnGUI() 
    {
        DisplayCoinsCount();
        DisplayRestartButton();
    }

    void DisplayCoinsCount() 
    {
	    Rect coinIconRect = new Rect(10, 10, 32, 32);
	    GUI.DrawTexture(coinIconRect, coinIconTexture);                         
		
	    GUIStyle style = new GUIStyle();
	    style.fontSize = 30;
	    style.fontStyle = FontStyle.Bold;
	    style.normal.textColor = Color.yellow;

	    Rect labelRect = new Rect(coinIconRect.xMax, coinIconRect.y, 60, 32);
	    GUI.Label(labelRect, coins.ToString(), style);
    }

    void DisplayRestartButton() 
    {
        if (dead && grounded) 
        {
            Rect buttonRect = new Rect(Screen.width * 0.35f, Screen.height * 0.45f, Screen.width * 0.30f, Screen.height * 0.1f);
	        if (GUI.Button(buttonRect, "Tap to restart!")) 
	        {
	        	
				SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
	        }
	    }
    }

  void AdjustFootstepsAndJetpackSound(bool jetpackActive) 
  {
      footstepsAudio.enabled = !dead && grounded;
      jetpackAudio.enabled =  !dead && !grounded;
	  jetpackAudio.volume = jetpackActive ? 1.0f : 0.5f;        
  }
}
