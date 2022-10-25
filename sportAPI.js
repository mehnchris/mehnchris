

fetch('https://sportspage-feeds.p.rapidapi.com/teams?league=%3CREQUIRED%3E', 
{
    method: 'GET',
    headers: 
    {
		'X-RapidAPI-Key': '42b2032926msh5dca9bab6f71337p15c103jsn3a7763a4264c',
		'X-RapidAPI-Host': 'sportspage-feeds.p.rapidapi.com'
	}
})
	.then(response => response.json())
	.then(response => console.log(response))
	.catch(err => console.error(err));
