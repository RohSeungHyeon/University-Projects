<?php
include_once ("./DBSETTING.php");
$score = @$_GET['MyScore'];
$sql = "select count(score) + 1 from RANK WHERE score>$score";
$result = mysqli_query($conn, $sql);
while($row = mysqli_fetch_assoc($result))
    echo $row['MyRank'];
?>