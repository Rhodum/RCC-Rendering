<?php
// $userid = $_GET['charapp'];
// $username = $_GET['name'];
// $userid = "http://finalb.xyz/Nobelium/public/user/getcharapp/2";
// $username = "beeznik";

$queue = file_get_contents("http://finalb.xyz/Nobelium/public/user/nobserv/getthumbnailqueue");

if ($queue == ""){
	exit();
}

$queuepeices = explode(";", $queue);


foreach ($queuepeices as $item) {
	$itembase = explode(":", $item);
	
	if ($itembase[0] == "asset"){
		$url = "http://finalb.xyz/Nobelium/public/user/nobserv/getthumbasset/";
	} elseif($itembase[0] == "user"){
		$url = "http://finalb.xyz/Nobelium/public/user/nobserv/getthumbcharapp/";
	}
	
	$itemid = $itembase[1];
	
	echo "loading thumbnail for" . $itemid;
	
	$fullurl = $url . $itemid;
	
	$serverscript = file_get_contents("file1.txt") . $fullurl . file_get_contents("file2.txt") . $itemid . file_get_contents("file3.txt");

	$myfile = fopen("gameserver.txt", "w") or die("Unable to open file!");
	$txt = $serverscript;
	fwrite($myfile, $txt);
	fclose($myfile);

	exec("Host.exe -console -placeid:18128");

	file_get_contents("http://finalb.xyz/Nobelium/public/user/nobserv/thumbqueueremove?id=" . $itemid . "&type=" . $itembase[0]);
	
	//unlink("gameserver.txt");
}


?>