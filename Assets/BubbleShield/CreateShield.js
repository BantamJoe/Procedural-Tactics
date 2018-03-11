#pragma strict

var player : Transform;
var bubbleShield : GameObject;
var bubbleSound : AudioClip;

private var cloneBubble : GameObject;
private var canSpawn : boolean = true;
private var timeUntilNextSpawn : float;

function Update()
{
	if(canSpawn && Input.GetKeyDown(KeyCode.B))
	{
		cloneBubble = Instantiate(bubbleShield);
		cloneBubble.transform.position = player.position + Vector3(0,-2,0);
		canSpawn = false;
		timeUntilNextSpawn = 0.0;
		GetComponent.<AudioSource>().PlayOneShot(bubbleSound);
	}
	
	if(!canSpawn)
	{
		timeUntilNextSpawn += Time.deltaTime;
	}
	
	if(timeUntilNextSpawn >= 7)
	{
		canSpawn = true;
		Destroy(cloneBubble);
		GetComponent.<AudioSource>().Stop();
	}
}