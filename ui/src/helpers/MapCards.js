import React from 'react';
import { Card } from 'antd';
const { Meta } = Card;

function MapCards(elements){
    const cards = elements.map((elem) => {
        return <Card
        hoverable
        key={elem.id}
        title={elem.title}
        type={"inner"}
        size={"small"}
        style={{width: 300}}
        cover={
           <img className="img-card-my"
           style={{margin: "0 auto"}}
           alt="No"
           src={`https://localhost:44382/${elem.cover}`}/> 
        }>
            <Meta description={cutDescr(elem.description)}/>
        </Card>        
    });
    return cards;
}

function cutDescr(descr){
    let index = descr.indexOf(".", 0);
    return descr.substr(0, index);
}

export default MapCards;