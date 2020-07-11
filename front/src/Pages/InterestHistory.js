import React, { PureComponent } from 'react';
import {
  BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend,
} from 'recharts';

export default class InterestHistory extends PureComponent {
    state = {
        history: []
    };

    componentDidMount() {
        if(this.props.token !== ""){
          fetch(`https://localhost:44337/api/accounts/${this.props.accountId}/interesthistory`,{
            headers: {
               authorization: "Bearer " + this.props.token
             }
          })
          .then(res => res.json())
          .then((data) => {
            let processed = data.map(row => {
              return {
                Month: row.timestamp.substring(0,7),
                Interest: row.value      
              };
            });
    
            this.setState({ history: processed });
          })
          .catch(console.log)
        }
    }

    render() {
    return (
        <BarChart
        width={1000}
        height={500}
        data={this.state.history}
        margin={{
            top: 5, right: 30, left: 20, bottom: 5,
        }}
        >
        <CartesianGrid strokeDasharray="3 3" />
        <XAxis dataKey="Month" />
        <YAxis />
        <Tooltip />
        <Legend />
        <Bar dataKey="Interest" fill="#8884d8" />
        </BarChart>
    );
    }
}
