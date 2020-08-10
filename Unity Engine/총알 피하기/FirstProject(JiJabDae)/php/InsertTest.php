<?php
include_once ("./DBSETTING.php");
$name = @$_GET['name'];
$score = @$_GET['score'];
$request_hash = @$_GET['hash'];
if(!$score)
    die("No score");
if(!$name)
    die("No name");
if(!$request_hash)
    die("No hash");
    
$data = $name."_".$score."_hash";
$hash = md5($data);
if(strtolower($hash) != strtolower($request_hash)){
    die("Invalid Request");
} else{
    mysqli_query($conn, "insert into RANK_TABLE(name, score) VALUES('$name', '$score')");
    echo $name."\n";
    echo $score."\n";
    echo "success";
}
?>