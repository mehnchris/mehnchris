

const navToggle= document.querySelector('.nav-toggle');
const navLinks= document.querySelectorAll('.nav__link')
navToggle.addEventListener('click', ()=> 
{
    document.body.classList.toggle('nav-open')
});

navLinks.forEach(link =>
{
    link.addEventListener('click', () =>
    {
        document.body.classList.remove('nav-open');
    })
})

var myDate = new Date();
var hrs = myDate.getHours();

var greeting;

if (hrs < 12)
greet = 'Good Morning';

else if (hrs >= 12 && hrs <= 17)
greet = 'Good Afternoon';

else if (hrs >= 17 && hrs <= 24)
greet = 'Good Evening';

document.getElementById('lblGreetings').innerHTML =
        '<b>' + greet + '</b>';

/* Start of greeting at to of Page*/
var myDate = new Date();
var hrs = myDate.getHours();

var greeting;

if (hrs < 12)
greet = 'Good Morning';

else if (hrs >= 12 && hrs <= 17)
greet = 'Good Afternoon';

else if (hrs >= 17 && hrs <= 24)
greet = 'Good Evening';


document.getElementById('lblGreetings').innerHTML =
        '<b>' + greet + '</b>';
/* End of greeting at to of Page*/