<?php include('../Site/init.php'); 

error_reporting(0);
$userid1 = $_GET["userid"];
$userid = filter_var($userid1, FILTER_SANITIZE_STRING);
$currentuserid = $userid;



$getUser = $handler->query("SELECT * FROM users WHERE id='".$currentuserid."'");
$gU = $getUser->fetch(PDO::FETCH_OBJ);
$usershirt1 = $gU->shirt;
$userpants = $gU->pants;

if ($userpants == ''){
    $userpants = "5381829246";
}


if ($usershirt1 == ''){
    $usershirt = "5381829246";
} else {
    $usershirt = $gU->shirt;
}

$userwearable = $gU->hat;

    $getrbxm = $handler->query("SELECT * FROM items WHERE wearable='".$userwearable."'");
    $gR = $getrbxm->fetch(PDO::FETCH_OBJ);

$gethattex1 = $gR->rcctex;
$gethatid1 = $gR->rccid;


if ($gethattex1 == ''){
    $gethattex = "6360376794";
} else {
    $gethattex = $gR->rcctex;
}

if ($gethatid1 == ''){
    $gethatid = "6360376794";
} else {
    $gethatid = $gR->rccid;
}



?>



{"torso":"<? echo $gU->TorsoColor ?>","leftarm":"<? echo $gU->LeftArmColor ?>","rightarm":"<? echo $gU->RightArmColor ?>","leftleg":"<? echo $gU->LeftLegColor ?>","rightleg":"<? echo $gU->RightLegColor ?>","head":"<? echo $gU->HeadColor ?>","shirt":"<? echo $usershirt ?>","pants":"<? echo $userpants ?>","hat":"<? echo $gethatid ?>","hattex":"<? echo $gethattex ?>","hatvector":"<? echo $gR->vectorpos ?>"}

