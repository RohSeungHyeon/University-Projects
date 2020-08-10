<?php
include_once ("./DBSETTING.php");
$sql = "select score, name, (select count(*) + 1 from RANK_TABLE WHERE score>t.score) AS rank from RANK_TABLE AS t ORDER BY rank ASC";
$result = mysqli_query($conn, $sql);
while($row = mysqli_fetch_assoc($result))
    echo $row['name'].','.$row['score'].','.$row['rank']."\n";
?>