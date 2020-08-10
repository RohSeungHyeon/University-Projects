<?php
include_once ("./DBSETTING.php");
$score = @$_GET['MyScore'];
$sql = "select count(score) as MyRank from RANK_TABLE where score>=$score";
$result = mysqli_query($conn, $sql);
while($row = mysqli_fetch_assoc($result))
    echo $row['MyRank'];
?>