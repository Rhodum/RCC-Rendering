<?php
include('../Site/init.php'); 

error_reporting(0);

$userid1 = $_GET["userid"];
$userid = filter_var($userid1, FILTER_SANITIZE_STRING);


// Read all
$json = file_get_contents('php://input');
$json = json_decode($json, true);
$pngimage = $json['png'];

$imageNo = uniqid(); 
$image = $pngimage;
$path = "img/$imageNo.png";




$success =  file_put_contents($path,base64_decode($image));



$avatar = ("http://rhodum.xyz/RCC/$path");


$handler->query("UPDATE users SET avatarurl='$avatar' WHERE id='$userid'");
$handler->query("DELETE FROM thumbnailque WHERE userid ='$userid'");
?>

success