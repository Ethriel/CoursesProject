import React from 'react';
import { Card } from 'antd';
const { Meta } = Card;

function MapCards(elements, onClickHandler) {
    const cards = elements.map((elem) => {
        const cover =
            <div className="img-container-my">
                <img className="img-card-my"
                    style={{ margin: "0 auto" }}
                    alt="No"
                    src={`https://localhost:44382/${elem.cover}`} />
            </div>
        return <Card
            hoverable
            key={elem.id}
            cardid={elem.id}
            title={elem.title}
            type={"inner"}
            size={"small"}
            style={{ width: 300 }}
            onClick={onClickHandler}
            cover={cover}>
            <Meta description={cutDescr(elem.description)} />
        </Card>
    });
    return cards;
}

function cutDescr(descr) {
    let index = descr.indexOf(".", 0);
    return descr.substr(0, index);
}

export default MapCards;