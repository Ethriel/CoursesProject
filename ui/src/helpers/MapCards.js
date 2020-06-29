import React from 'react';
import GetInfoArr from './GetInfoArr';
import { Card } from 'antd';
const { Meta } = Card;

function MapCards(quantity){
    const info = GetInfoArr(quantity);
        const cards = info.map((item) => {
            return <Card
                key={item.key}
                title={item.title}
                cover={
                    <img className="img-card-my"
                        alt="Example"
                        src={require("../img/logo.png")} />
                }>
                <Meta
                    description={item.description} />
            </Card>
        });

        return cards;
}

export default MapCards;