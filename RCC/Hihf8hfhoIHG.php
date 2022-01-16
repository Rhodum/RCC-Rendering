<?php include('../Site/init.php'); 

error_reporting(0);

$q = $handler->query("SELECT * FROM thumbnailque ORDER BY userid DESC LIMIT 1");
$row = $q->fetch();

$quecheck = $row['userid'];

if ($quecheck == ''){
    exit('{"userid":"null","charapp":"http://rhodum.xyz/RCC/charget.php?id=1","status":"true"}');
}

?>
{"userid":"<? echo $row['userid']; ?>","charapp":"<? echo $row['thumbnail']; ?>","status":"false"}