<?php
include_once ("./DBSETTING.php");
$sql = "select MAX(score) as max_score, name from RANK_TABLE GROUP BY id ORDER BY max_score DESC";
$result = mysqli_query($conn, $sql);
while($row = mysqli_fetch_assoc($result))
    echo $row['name'].','.$row['max_score']."\n";
?>