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
using System.Collections;

public class LaserScript : MonoBehaviour 
{
    public Sprite laserOnSprite;    
    public Sprite laserOffSprite;
    public float interval = 0.5f;    
    public float rotationSpeed = 0.0f;
    private bool isLaserOn = true;    
    private float timeUntilNextToggle;

    void Start () 
    {
        timeUntilNextToggle = interval;
    }

    void FixedUpdate () 
    {
        timeUntilNextToggle -= Time.fixedDeltaTime;
        if (timeUntilNextToggle <= 0) 
        {
            isLaserOn = !isLaserOn;
            GetComponent<Collider2D>().enabled = isLaserOn;
            SpriteRenderer spriteRenderer = ((SpriteRenderer)this.GetComponent<Renderer>());
            if (isLaserOn) 
            {
                spriteRenderer.sprite = laserOnSprite;
			}
            else
            {
                spriteRenderer.sprite = laserOffSprite;
            }
            timeUntilNextToggle = interval;
        }
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time. fixedDeltaTime);
     }

}
