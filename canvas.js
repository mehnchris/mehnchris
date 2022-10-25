console.log("loaded canvas.js")

/* Build the properties of the game */
var canvas = document.getElementById("myCanvas");
var ctx= canvas.getContext("2d");
var X= canvas.width / 2;
var Y= canvas.height - 40;
var dx= 2;
var dy= -2;
var ballRadius= 10;
var paddleHeight= 10;
var paddleWidth= 75;
var paddleX= (canvas.width - paddleWidth) /2;
var rightPressed= false;
var leftPressed= false;
var brickRowCount= 4;
var brickColumnCount= 13;
var brickWidth= 70;
var brickHeight= 20;
var brickPaddling= 10;
var brickOffsetTop= 30;
var brickOffsetLeft= 30;
var bricks=[];

/* All starts at 0, c represents the column count of the bricks and r represents the row count of the bricks*/
for (var c=0; c<brickColumnCount; c++)
{
    bricks[c]=[];
    for (var r=0; r<brickRowCount; r++)
    {
        bricks[c][r]= { x:0, y:0, status: 1};
    }
}

/** determine how the game works, score starts at 0 and the number of lives starts at 3*/
var score=0;
var lives=3;

/** Won't be using the key up nor key down for this game but you will be able to use the "E"*/
document.addEventListener("keydown", keyDownHandler, false);
document.addEventListener("keyup", keyUpHandler, false);

/** Set functions for my handdlers and detectors*/
function keyDownHandler(e)
{
    if (e.key=="Right" || e.key =="ArrowRight")
    {
        rightPressed= true;
    }
    else if (e.key=="left" || e.key== "ArrowLeft")
    {
        leftPressed= true;
    }
}
function keyUpHandler(e)
{
    if (e.key== "Right" || e.key=="ArrowRight")
    {
        rightPressed= false;
    }
    else if(e.key== "Left" || e.key=="ArrowLeft")
    {
        leftPressed= false;
    }
}
function collisionDectection()
{
    for (var c=0; c<brickColumnCount; c++)
    {
        for (var r= 0; r<brickRowCount; r++)
        {
            var b=bricks[c][r];
            if (b.status==1)
            if (c > b.X && X< b.X + brickWidth && Y >b.Y && Y < b.Y + brickHeight)
            {
                dy= -dy;
                b.status= 0;
                score++;

                if (score ==brickRowCount * brickColumnCount)
                {
                    alert("Hey congrats, you did it!");
                    document.location.reload();
                }
            }
        }
    }
}
function scoreDraw()
{
    ctx.font= '20px Arial';
    ctx.fillStyle= '#0095DD';
    ctx.fillText("Score: " +score, 8,20);
}
function drawBricks()
{
    for (var c=0; c< brickColumnCount; c++)
    {
        for (var r=0; r<brickRowCount; c++)
        {
            if (bricks[c][r].status==1)
            {
                var brickX= (c * (brickWidth + brickPaddling)) + brickOffsetLeft;
                var brickY= (r * (brickHeight + brickPaddling)) + brickOffsetTop;

                bricks[c][r].X= brickX;
                bricks[c][r].Y= brickY;

                ctx.beginPath();
                ctx.rect(brickX, brickY, brickWidth, brickHeight);
                ctx.fillStyle= "#0095DD"
                ctx.fill();
                ctx.closePath();
            }
        }
    }
}
function drawLives()
{
    ctx.font= "16px Arial";
    ctx.fillStyle= "#0095DD"
    ctx.fillText("Lives: " +lives, canvas.width-65, 20);
}
function drawBall()
{
    ctx.beginPath();
    ctx.arc(X, Y, 10, 0, Math.PI * 2);
    ctx.fillStyle= "#0095DD";
    ctx.fill();
    ctx.closePath();
}
function drawPaddle()
{
    ctx.beginPath();
    ctx.rect(paddleX, canvas.height-paddleHeight, paddleWidth, paddleHeight);
    ctx.fillStyle= "#0095DD";
    ctx.fill();
    ctx.closePath();
}


/*keyUpHandler()
keyDownHandler()*/
drawPaddle()
drawBall()
drawLives()
drawBricks()
scoreDraw()
collisionDectection()
