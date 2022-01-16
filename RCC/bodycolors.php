<?php
$string = 
'<?xml version="1.0" encoding="utf-8" ?>
<roblox xmlns:xmime="http://www.w3.org/2005/05/xmlmime" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="http://rhodum.xyz/roblox.xsd" version="4">
	<External>null</External>
	<External>nil</External>
	<Item class="BodyColors">
		<Properties>
			<int name="HeadColor">23</int>
			<int name="LeftArmColor">23</int>
			<int name="LeftLegColor">23</int>
			<string name="Name">Body Colors</string>
			<int name="RightArmColor">23</int>
			<int name="RightLegColor">23</int>
			<int name="TorsoColor">23</int>
			<bool name="archivable">true</bool>
		</Properties>
	</Item>
</roblox>
';

echo '<pre>', htmlentities($string), '</pre>';

?>


