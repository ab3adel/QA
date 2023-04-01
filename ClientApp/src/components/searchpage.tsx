import React from 'react';
import { RouteProps } from 'react-router-dom'
import { QuestionData ,searchQuestions} from './questiondata';
import {Page} from './page'
import { QuestionList } from './questionlist';
export const SearchPage =(props:RouteProps)=>{
    var search: string|undefined;
    if (props.location){
    search =props.location.search.substr(1)
    }
    const [
      questions,
      setQuestions,
    ] = React.useState<QuestionData[]>([]);
  
    React.useEffect(() => {
        const doSearch = async (criteria: string) => {    
          const foundResults = await searchQuestions(   
            criteria,   
          );
      
          setQuestions(foundResults);
      
        };
      if (search){
        doSearch(search);
      } 
      
      }, [search]);
      
      
    return (
        <Page title="Search Results">

  {search && (

    <p >

      for "{search}"

    </p>

  )}
<QuestionList data={questions}></QuestionList>
</Page>
    )
}