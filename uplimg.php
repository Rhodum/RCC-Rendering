<?php
    $apikey = $_GET['apikey'];
    if ($apikey !== "jv9BkLv8TFfcs67q"){
        exit();
    }else{
        $yes = 'yes';
    }
    $image64 = file_get_contents('php://input');
    $userid = $_GET['uid'];
    $image = base64_decode($image64);
    $type = $_GET['typeofasset'];
    if ($type == "character"){
        $type = "user";
    }
    if (file_exists("../imageServer/$type/$userid.png")){
        unlink("../imageServer/$type/$userid.png");
        $file = "../imageServer/$type/$userid.png";
        file_put_contents($file, $image);
    } else {   
        $file = "../imageServer/$type/$userid.png";
        file_put_contents($file, $image);
    }

?>