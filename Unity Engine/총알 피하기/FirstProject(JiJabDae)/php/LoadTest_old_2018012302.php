<?php
include_once ("./DBSETTING.php");
$sql = "select score, name from RANK_TABLE ORDER BY score DESC";
$result = mysqli_query($conn, $sql);
while($row = mysqli_fetch_assoc($result))
    echo $row['name'].','.$row['score']."\n";
?>